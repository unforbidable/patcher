<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:variable name="header">
    <tr>
      <td class="td-name">
        Name
      </td>
      <td class="td-description">
        Description
      </td>
    </tr>
  </xsl:variable>

  <xsl:template match="index | node()">

    <html>
      <head>
        <title>
          Index
        </title>
      </head>
      <body>

        <div class="div-title">
          Index
        </div>

        <div class="div-header">
          Helpers
        </div>
        <div class="div-summary">
          Global classes that provide various functionality.
        </div>
        <div class="div-table">
          <table>
            <xsl:copy-of select="$header"/>
            <xsl:for-each select="type[@category='Helpers']">
              <tr>
                <td class="td-name">
                  <see>
                    <xsl:attribute name="cref">
                      <xsl:value-of select="@fullname"/>
                    </xsl:attribute>
                    <xsl:value-of select="@name"/>
                  </see>
                </td>
                <td class="td-description">
                  <xsl:copy-of select="summary"/>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>
        
        <div class="div-header">
          Forms
        </div>
        <div class="div-summary">
          Classes representing forms and collections of forms.
        </div>
        <div class="div-table">
          <table class="table-main">
            <xsl:copy-of select="$header"/>
            <xsl:for-each select="type[@category='Forms']">
              <tr>
                <td class="td-name">
                  <see>
                    <xsl:attribute name="cref">
                      <xsl:value-of select="@fullname"/>
                    </xsl:attribute>
                    <xsl:value-of select="@name"/>
                  </see>
                </td>
                <td class="td-description">
                  <xsl:copy-of select="summary"/>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <div class="div-header">
          Fields
        </div>
        <div class="div-summary">
          Classes representing form fields and collections of form fields.
        </div>
        <div class="div-table">
          <table class="table-main">
            <xsl:copy-of select="$header"/>
            <xsl:for-each select="type[@category='Fields']">
              <tr>
                <td class="td-name">
                  <see>
                    <xsl:attribute name="cref">
                      <xsl:value-of select="@fullname"/>
                    </xsl:attribute>
                    <xsl:value-of select="@name"/>
                  </see>
                </td>
                <td class="td-description">
                  <xsl:copy-of select="summary"/>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

        <div class="div-header">
          Constants
        </div>
        <div class="div-summary">
          Global classes that define various constant values.
        </div>
        <div class="div-table">
          <table class="table-main">
            <xsl:copy-of select="$header"/>
            <xsl:for-each select="type[@category='Constants']">
              <tr>
                <td class="td-name">
                  <see>
                    <xsl:attribute name="cref">
                      <xsl:value-of select="@fullname"/>
                    </xsl:attribute>
                    <xsl:value-of select="@name"/>
                  </see>
                </td>
                <td class="td-description">
                  <xsl:copy-of select="summary"/>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </div>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
