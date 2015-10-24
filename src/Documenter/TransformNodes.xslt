<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:variable name="fullname" select="html/head/meta[@name='doc-fullname']/@content" />
  <xsl:variable name="depth" select="string-length($fullname) - string-length(translate($fullname,'.',''))" />
  <xsl:variable name="relativePath" select="substring('../../../', 1, $depth * 3)" />

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <!-- c => span-code -->
  <xsl:template match="*/c">
    <span class="span-code">
      <xsl:apply-templates select="@*|node()"/>
    </span>
  </xsl:template>

  <!-- see cref => a href -->
  <xsl:template match="*/see">
    <a>
      <xsl:attribute name="href">
        <xsl:variable name="fullName" select="@cref"/>
        <xsl:value-of select="concat($relativePath, translate($fullName, '.', '/'),'.html')"/>
      </xsl:attribute>
      <xsl:copy-of select="."/>
      <xsl:if test=". = ''">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string" select="@cref" />
          <xsl:with-param name="delimiter" select="'.'" />
        </xsl:call-template>
      </xsl:if>
    </a>
  </xsl:template>

  <!-- Insert css -->
  <xsl:template match="html/head/title">
    <xsl:copy-of select="." />
    <link rel="stylesheet" type="text/css">
      <xsl:attribute name="href">
        <xsl:value-of select="concat('../', $relativePath, 'doc-styles.scss')"/>
      </xsl:attribute>
    </link>
  </xsl:template>
  
</xsl:stylesheet>
