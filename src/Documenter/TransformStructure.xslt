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

  <xsl:variable name="typeName" select="@name" />

  <xsl:template match="type | node()">

    <html>
      <head>
        <title>
          <xsl:value-of select="@name"/>
        </title>
        <meta name="doc-fullname">
          <xsl:attribute name="content">
            <xsl:value-of select="@fullname"/>
          </xsl:attribute>
        </meta>
      </head>
      <body>

        <div class="div-title">
          <xsl:value-of select="@name"/>
          <xsl:if test="string-length(@gametitle) > 0">
            <xsl:value-of select="concat(' (', @gametitle, ')')"/>
          </xsl:if>
        </div>

        <div class="div-header">
          Summary
        </div>
        <div class="div-summary">
          <xsl:copy-of select="summary"/>
        </div>

        <xsl:if test="field">
          <div class="div-header">
            Fields
          </div>
          <div class="div-table">
            <table>
              <xsl:copy-of select="$header" />
              <xsl:for-each select="field">
                <tr>
                  <td class="td-name">
                    <xsl:value-of select="@name"/>
                  </td>
                  <td class="td-description">
                    <xsl:copy-of select="summary"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:if>

        <xsl:if test="property">
          <div class="div-header">
            Properties
          </div>
          <div class="div-table">
            <table>
              <xsl:copy-of select="$header" />
              <xsl:for-each select="property">
                <tr>
                  <td class="td-name">
                    <xsl:copy-of select="signature"/>
                  </td>
                  <td class="td-description">
                    <xsl:copy-of select="summary"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:if>

        <xsl:if test="method and method/@extension = 'false'">
          <div class="div-header">
            Methods
          </div>
          <div class="div-table">
            <table>
              <xsl:copy-of select="$header" />
              <xsl:for-each select="method[@extension = 'false']">
                <tr>
                  <td class="td-name">
                    <xsl:copy-of select="signature"/>
                  </td>
                  <td class="td-description">
                    <xsl:copy-of select="summary"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:if>

        <xsl:if test="method and method/@extension = 'true'">
          <div class="div-header">
            Extension Methods
          </div>
          <div class="div-table">
            <table>
              <xsl:copy-of select="$header" />
              <xsl:for-each select="method[@extension = 'true']">
                <tr>
                  <td class="td-name">
                    <xsl:copy-of select="signature"/>
                  </td>
                  <td class="td-description">
                    <xsl:copy-of select="summary"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:if>

        <xsl:if test="remarks">
          <div class="div-header">
            Remarks
          </div>
          <div class="div-summary">
            <xsl:copy-of select="remarks"/>
          </div>
        </xsl:if>

        <hr />
        <div class="div-navigation">
          Return to: <see cref="Index">Index</see>
        </div>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
