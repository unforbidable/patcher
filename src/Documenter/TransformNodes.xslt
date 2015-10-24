<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:variable name="fullname" select="html/body/div[@class='div-subtitle']" />
  <xsl:variable name="depth" select="string-length($fullname) - string-length(translate($fullname,'.',''))" />
  <xsl:variable name="relativePath" select="substring('../../../', 1, $depth * 3)" />
  
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="*/c">
    <span class="span-code">
      <xsl:apply-templates select="@*|node()"/>
    </span>
  </xsl:template>

  <xsl:template match="*/see">
    <a>
      <xsl:apply-templates select="@*|node()"/>
    </a>
  </xsl:template>
  
  <xsl:template match="*/see/@cref">
    <xsl:attribute name="href">
      <xsl:variable name="fullName" select="."/>
      <xsl:value-of select="concat($relativePath, translate($fullName, '.', '/'),'.html')"/>
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="html/head/title">
    <xsl:copy-of select="." />
    <link rel="stylesheet" type="text/css">
      <xsl:attribute name="href">
        <xsl:value-of select="concat('../', $relativePath, 'doc-styles.scss')"/>
      </xsl:attribute>
    </link>
  </xsl:template>
  
</xsl:stylesheet>
